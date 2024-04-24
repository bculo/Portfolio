

export function getAccessToken(authority: string, clientId: string): string {
    const storageItemString = localStorage.getItem(`oidc.user:${authority}:${clientId}`)
    if(!storageItemString) {
        return '';
    }
    try {
        return JSON.parse(storageItemString!).access_token
    }
    catch {
        return ''
    }
}