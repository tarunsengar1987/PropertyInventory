import { Injectable } from '@angular/core';
import { MessageService } from 'primeng/api';

@Injectable({
    providedIn: 'root'
})

export class NotificationService {
    constructor(private _messageService: MessageService) { }

    success(message: string | string[]) {
        this._messageService.add({ severity: 'success', summary: 'Successful', detail: message.toString(), life: 3000 });
    }

    error(message: string | string[]) {
        this._messageService.add({ severity: 'error', summary: 'Error', detail: message.toString(), life: 3000 });
    }
}