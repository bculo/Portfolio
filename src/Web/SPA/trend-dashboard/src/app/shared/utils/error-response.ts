import { HttpErrorResponse } from "@angular/common/http";

export function isValidationException(response: HttpErrorResponse ) {
    if(response.error && response.error.title && response.error.title == 'One or more validation errors occurred.' 
        && response.status == 400 && response.error.errors) 
    {
        return true
    }

    return false;
}

export function getMessageFromBadRequest(response: HttpErrorResponse){
    return (response.error && response.error.message) ? response.error.message : response.message
}