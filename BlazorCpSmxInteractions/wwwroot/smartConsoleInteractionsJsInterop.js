import SmartConsoleInteractions from './js/smartConsoleInteractions.js';


// This is a JavaScript module that is loaded on demand. It can export any number of
// functions, and may import other JavaScript modules if required.


// query - Execute query to get objects from the Security Management API.
export async function smxQuery(queryRequestId, queryRequestParams, guid, demoInteractionModulPath) {
    const demoInteraction = await importdemoInteractionModulModule(demoInteractionModulPath);

    let interactions;

    if (demoInteraction != 'undefined' && demoInteraction) {
        interactions = new SmartConsoleInteractions(demoInteraction);
    }
    else {
        interactions = new SmartConsoleInteractions();
    }

    return await interactions.query(queryRequestId, queryRequestParams, guid);
}


// getContextObject -Extension context provided by SmartConsole.
export async function smxGetContextObject(guid, demoInteractionModulPath) {
    const demoInteraction = await importdemoInteractionModulModule(demoInteractionModulPath);

    let interactions;

    if (demoInteraction != 'undefined' && demoInteraction) {
        interactions = new SmartConsoleInteractions(demoInteraction);
    }
    else {
        interactions = new SmartConsoleInteractions();
    }

    return await interactions.getContextObject(guid);
}


// requestCommit - Request SmartConsole user to execute list of commands. 
// Used by extensions to apply changes by SmartConsole user private session.
export async function smxRequestCommit(commands, guid, demoInteractionModulPath) {
    const demoInteraction = await importdemoInteractionModulModule(demoInteractionModulPath);

    let interactions;

    if (demoInteraction != 'undefined' && demoInteraction) {
        interactions = new SmartConsoleInteractions(demoInteraction);
    }
    else {
        interactions = new SmartConsoleInteractions();
    }

    return await interactions.requestCommit(commands, guid);
}


// navigate - Request SmartConsole to navigate to a rule.
export async function smxNavigate(uid, guid, demoInteractionModulPath) {
    const demoInteraction = await importdemoInteractionModulModule(demoInteractionModulPath);

    let interactions;

    if (demoInteraction != 'undefined' && demoInteraction) {
        interactions = new SmartConsoleInteractions(demoInteraction);
    }
    else {
        interactions = new SmartConsoleInteractions();
    }

    return await interactions.navigate(uid, guid);
}


// closeExtensionWindow - Request SmartConsole to close the extension window.
export async function smxCloseExtensionWindow(guid, demoInteractionModulPath) {
    const demoInteraction = await importdemoInteractionModulModule(demoInteractionModulPath);

    let interactions;

    if (demoInteraction != 'undefined' && demoInteraction) {
        interactions = new SmartConsoleInteractions(demoInteraction);
    }
    else {
        interactions = new SmartConsoleInteractions();
    }

    return await interactions.closeExtensionWindow(guid);
}


async function importdemoInteractionModulModule(modulePath) {
    let demoInteraction;

    if (typeof modulePath != 'undefined' && modulePath) {

        try {
            const module = await import(modulePath);

            demoInteraction = module.default;
        } catch (e) {
            console.warn("demoInteraction Module-Import-Warning: " + e.message);
        };
    }

    return demoInteraction;
}


// Get a user agent with the navigator.userAgent property.
export async function getUserAgent() {
    return navigator.userAgent;
}
