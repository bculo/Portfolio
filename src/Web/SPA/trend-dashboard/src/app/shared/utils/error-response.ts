import { HttpErrorResponse } from "@angular/common/http";
import { FormHelperService } from "src/app/services/form-mapper/form-helper.service";
import { NotificationService } from "src/app/services/notification/notification.service";

export function isValidationException(response: HttpErrorResponse ) {
    if(response.error 
        && response.error.title 
        && response.error.title == 'One or more validation errors occurred.' 
        && response.status == 400 && response.error.errors) 
    {
        return true
    }

    return false;
}

export function getMessageFromBadRequest(response: HttpErrorResponse){
    if(response.status === 0)
        return "Unknown error";
    return (response.error && response.error.message) ? response.error.message : response.message
}

export function handleServerError(response: HttpErrorResponse, 
    notification: NotificationService | null = null, 
    helper: FormHelperService | null = null, 
    formIdentifier: string | null = null): string {

    if(isValidationException(response) && helper && formIdentifier) 
        helper.handleValidationError(formIdentifier, response.error.errors);

    const errorMessage = getMessageFromBadRequest(response);

    if(notification)
        notification.error(errorMessage);
    
    return errorMessage;
}