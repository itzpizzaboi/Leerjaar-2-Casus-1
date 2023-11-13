export async function getCurrentUser() {
    const r = await fetch('/api/user');
    const json = await r.json();

    return json;
}

export async function deleteCurrentUser() {
    const r = await fetch('/api/user', { method: 'DELETE' });
    const success = await r.json();

    return success;
}

export async function isUserLoggedIn() {
    try {
        await getCurrentUser();
        return true;
    } catch (err) {
        return false;
    }
}

function redirectToAuth() {
    const oldUrl = encodeURIComponent(window.location.pathname + window.location.search);
    window.location = `/auth/login?redirectUrl=${oldUrl}`;
}

export function redirectToHome() {
    window.location = "/";
}

export async function redirectIfNotAuth() {
    const isLoggedIn = await isUserLoggedIn();
    if (isLoggedIn)
        return;

    redirectToAuth();
}

export async function redirectIfNotOrganizer() {
    await redirectIfNotAuth();

    const user = await getCurrentUser();
    if (user.role == 1)
        return;

    redirectToHome();
}

export async function doUserLogin(email, password) {
    // pray that password does not contain ?, & and = lol
    const r = await fetch(`/api/auth/login?email=${email}&password=${password}`, { method: 'PUT' });
    const success = await r.json();

    return success;
}

export async function doUserSignup(email, password, role) {
    try {
        // pray that password does not contain ?, & and = lol
        const r = await fetch(`/api/auth/signup?email=${email}&password=${password}&role=${role}`, { method: 'POST' });
        const success = await r.json();
        return success;
    } catch {
        return false
    }
}

export async function doUserLogout() {
    // pray that password does not contain ?, & and = lol
    const r = await fetch(`/api/auth/logout`, { method: 'DELETE' });
    const success = await r.json();

    return success;
}

export async function canUserEditEvent(event) {
    const user = await getCurrentUser();
    if (user.events.filter(e => (e.id == event.id)) == -1)
        return false;

    return true;
}