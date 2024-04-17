import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class MessageService {
  constructor(private messageService: MessageService) {}
  showSucceess() {
    // this.messageService.add({severity:'success', summary:'Service Message', detail:'Via MessageService'});
  }

  showError() {
    // this.messageService.add([{severity:'success', summary:'Service Message', detail:'Via MessageService'},
    //                 {severity:'info', summary:'Info Message', detail:'Via MessageService'}]);
  }
}
