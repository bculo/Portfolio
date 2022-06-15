export function isValidationException(response: any) {
    if(response.error && response.error.title && response.error.title == 'One or more validation errors occurred.' 
        && response.status == 400 && response.error.errors) 
    {
        return true
    }

    return false;
}