import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { catchError, throwError } from 'rxjs';
import { extractErrorMessages } from '../utils/ApiError';
import { inject } from '@angular/core';
import { MessageService } from 'primeng/api';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const messageService = inject(MessageService);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      const messages = extractErrorMessages(error);

      messageService.add({
        severity: 'error',
        summary: 'Error',
        detail: messages.join('\n'),
      });

      console.error('HTTP Error:', error);
      return throwError(() => error);
    })
  );
};
