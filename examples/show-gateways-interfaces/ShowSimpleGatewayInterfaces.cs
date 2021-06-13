using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Text.Json;
using System.Xml;


namespace ShowGatewaysInterfaces
{
    // internal topology types
    public enum InternalTopologyType
    {
        NotDefined,                         // "not defined",
        DefinedByInterfaceIpAndNetMask,     // "network defined by the interface ip and net mask",
        DefinedByRouting,                   // "network defined by routing",
        Specific                            // "specific"
    }


    // Topology configuration
    public enum TopologyConf
    {
        External,   // "external",
        Internal,   // "internal",
        Automatic   // "automatic"
    }


    public class ShowSimpleGatewayInterfaces
    {
        // svg path for interface icon
        private const string _interfaceIcon = "M10.000,8.000 L10.000,7.000 L7.000,7.000 L6.000,7.000 L3.000,7.000 L3.000,8.000 L2.000,8.000 L2.000,7.000 L2.000,6.000 L3.000,6.000 L6.000,6.000 L6.000,5.000 L7.000,5.000 L7.000,6.000 L10.000,6.000 L11.000,6.000 L11.000,7.000 L11.000,8.000 L10.000,8.000 ZM8.000,4.000 L7.000,4.000 L7.000,4.000 L6.000,4.000 L6.000,4.000 L5.000,4.000 C4.448,4.000 4.000,3.552 4.000,3.000 L4.000,1.000 C4.000,0.447 4.448,-0.000 5.000,-0.000 L8.000,-0.000 C8.552,-0.000 9.000,0.447 9.000,1.000 L9.000,3.000 C9.000,3.552 8.552,4.000 8.000,4.000 ZM5.000,10.000 L5.000,12.000 C5.000,12.552 4.552,13.000 4.000,13.000 L1.000,13.000 C0.448,13.000 -0.000,12.552 -0.000,12.000 L-0.000,10.000 C-0.000,9.447 0.448,9.000 1.000,9.000 L1.000,9.000 L1.000,9.000 L4.001,9.000 C4.553,9.001 5.000,9.448 5.000,10.000 ZM9.000,9.000 L9.000,9.000 L9.000,9.000 L12.001,9.000 C12.553,9.001 13.000,9.448 13.000,10.000 L13.000,12.000 C13.000,12.552 12.552,13.000 12.000,13.000 L9.000,13.000 C8.448,13.000 8.000,12.552 8.000,12.000 L8.000,10.000 C8.000,9.447 8.448,9.000 9.000,9.000 Z";

        private static Dictionary<InternalTopologyType, string> _internalTopologyTypeString = new Dictionary<InternalTopologyType, string>()
        {
            { InternalTopologyType.NotDefined, "not defined" },
            { InternalTopologyType.DefinedByInterfaceIpAndNetMask, "network defined by the interface ip and net mask" },
            { InternalTopologyType.DefinedByRouting, "network defined by routing" },
            { InternalTopologyType.Specific, "specific" }
        };

        private static Dictionary<TopologyConf, string> _topologyConfString = new Dictionary<TopologyConf, string>()
        {
            { TopologyConf.External, "external" },
            { TopologyConf.Internal, "internal" },
            { TopologyConf.Automatic, "automatic" }
        };


        public static SimpleGatewayInterface[] GetInterfaces(JsonElement jsonInterfaces)
        {
            var gatewayInterfaces = new List<SimpleGatewayInterface>();

            JsonElement.ArrayEnumerator interfacesArrayEnumerator = jsonInterfaces.EnumerateArray();

            while (interfacesArrayEnumerator.MoveNext())
            {
                JsonElement currInterface = interfacesArrayEnumerator.Current;

                SimpleGatewayInterface @interface = new SimpleGatewayInterface();

                @interface.SvgIcon = GetIcon(currInterface);
                @interface.Name = GetName(currInterface);
                @interface.Topology = GetTopology(currInterface);
                @interface.IP = GetIp(currInterface);
                @interface.Comments = GetComments(currInterface);

                gatewayInterfaces.Add(@interface);
            }

            // sort interfaces by name (ASC order)
            gatewayInterfaces.Sort((a, b) => string.CompareOrdinal(a.Name.ToLower(), b.Name.ToLower()));

            return gatewayInterfaces.ToArray();
        }


        // get interface name
        private static string GetName(JsonElement jsonInterface)
        {
            string nameValue = string.Empty;

            if (jsonInterface.TryGetProperty("name", out var nameNode))
            {
                nameValue = nameNode.GetString();
            }

            return nameValue;
        }


        // get interface icon color
        private static string GetColor(JsonElement jsonInterface)
        {
            string colorValue = string.Empty;

            if (jsonInterface.TryGetProperty("color", out var colorNode))
            {
                colorValue = colorNode.GetString();
            }

            return colorValue;
        }


        // get interface icon
        private static MarkupString GetIcon(JsonElement jsonInterface)
        {
            string svgString = string.Empty;


            string iconValue = string.Empty;

            if (jsonInterface.TryGetProperty("icon", out var iconValueNode))
            {
                iconValue = iconValueNode.GetString();
            }


            if (iconValue == "NetworkObjects/network") 
            {
                XmlDocument document = new XmlDocument();

                var xmlns = "http://www.w3.org/2000/svg";

                var svg = document.CreateElement("svg", xmlns);
                svg.SetAttribute("class", "icon");

                var path = document.CreateElement("path", xmlns);
                path.SetAttribute("d", _interfaceIcon);
                // fill icon color
                path.SetAttribute("fill", Colors.ColorTable[GetColor(jsonInterface)]);
                path.SetAttribute("transform", "translate(1, 1)");

                svg.AppendChild(path);

                svgString = svg.OuterXml;
            }

            return new MarkupString(svgString);
        }


        // get internal topology string to topology column.
        private static string GetInternalTopology(JsonElement jsonInterface)
        {
            string internalTopologyString = string.Empty;

            if (jsonInterface.TryGetProperty("topology-settings", out var topologySettingsNode))
                if (topologySettingsNode.TryGetProperty("ip-address-behind-this-interface", out var ipAddressBehindThisInterfaceNode))
                {
                    var type = ipAddressBehindThisInterfaceNode.GetString();

                    if (type == _internalTopologyTypeString[InternalTopologyType.NotDefined])
                        internalTopologyString = "Undefined";
                    else if (type == _internalTopologyTypeString[InternalTopologyType.DefinedByInterfaceIpAndNetMask])
                        internalTopologyString = "This network";
                    else if (type == _internalTopologyTypeString[InternalTopologyType.DefinedByRouting])
                        internalTopologyString = "Defined by routes";
                    else if (type == _internalTopologyTypeString[InternalTopologyType.Specific])
                    {
                        if (topologySettingsNode.TryGetProperty("specific-network", out var specificNetworkNode))
                            internalTopologyString = specificNetworkNode.GetString();
                    }
                }

            return internalTopologyString;
        }


        // get external topology string to topology column.
        private static string GetExternalTopology()
        {
            return "External";
        }


        // get topology interface
        // When topology is external(manual or automatic) return external.
        // When topology is internal (manual or automatic) return ip address behind this interface (Undefined, This network, Defined by routes or specific network).
        private static string GetTopology(JsonElement jsonInterface)
        {
            string topologyString = string.Empty;

            var automaticInternal = false;

            if (jsonInterface.TryGetProperty("topology", out var topologyNode))
            {
                var topology = topologyNode.GetString();

                if (topology == _topologyConfString[TopologyConf.External])
                {
                    topologyString = GetExternalTopology();
                }

                if (topology == _topologyConfString[TopologyConf.Automatic])
                {
                    if (jsonInterface.TryGetProperty("topology-automatic-calculation", out var topologyAutomaticCalculationNode))
                    {
                        var topologyAutomaticCalculation = topologyAutomaticCalculationNode.GetString();

                        if (topologyAutomaticCalculation == _topologyConfString[TopologyConf.External])
                            topologyString = GetExternalTopology();
                        else
                            automaticInternal = true;
                    }
                }

                if ((topology == _topologyConfString[TopologyConf.Internal]) || automaticInternal)
                {
                    topologyString = GetInternalTopology(jsonInterface);
                }
            }

            return topologyString;
        }


        // get IP string to show at IP column.
        private static string GetIpString(string address, string maskLength)
        {
            var ip = address + "/" + maskLength;

            return ip;
        }


        // get ip and mask length
        private static string GetIp(JsonElement jsonInterface)
        {
            string ipString = string.Empty;

            if (jsonInterface.TryGetProperty("ipv4-address", out var ipv4AddressNode))
            {
                if (jsonInterface.TryGetProperty("ipv4-mask-length", out var ipv4MaskLengthNode))
                    ipString = GetIpString(ipv4AddressNode.GetString(), ipv4MaskLengthNode.GetInt32().ToString());
            }
            else if (jsonInterface.TryGetProperty("ipv6-address", out var ipv6AddressNode))
            {
                if (jsonInterface.TryGetProperty("ipv6-mask-length", out var ipv6MaskLengthNode))
                    ipString = GetIpString(ipv6AddressNode.GetString(), ipv6MaskLengthNode.GetInt32().ToString());
            }

            return ipString;
        }


        // get interface comments
        private static string GetComments(JsonElement jsonInterface)
        {
            string commentsValue = string.Empty;

            if (jsonInterface.TryGetProperty("comments", out var commentsNode))
            {
                commentsValue = commentsNode.GetString();
            }

            return commentsValue;
        }


        // create div for icon and interface name.
        private static RenderFragment GetIconAndName(JsonElement jsonInterface)
        {
            RenderFragment iconAndName;

            iconAndName = builder =>
            {
                var icon = GetIcon(jsonInterface);

                builder.OpenElement(0, "div");
                builder.AddContent(1, icon);

                var name = GetName(jsonInterface);

                builder.OpenElement(2, "div");
                builder.AddAttribute(3, "class", "name");
                builder.AddContent(4, name);
                builder.CloseElement();

                builder.CloseElement();
            };

            return iconAndName;
        }
    }


    public class SimpleGatewayInterface
    {
        public MarkupString SvgIcon { get; set; }

        public string Name { get; set; }

        public string Topology { get; set; }

        public string IP { get; set; }

        public string Comments { get; set; }
    }
}
