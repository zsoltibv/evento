import { inject, Injectable } from '@angular/core';
import { RestApiService } from './rest-api-service';
import {
  GenerateDescriptionRequest,
  GenerateDescriptionResponse,
} from '../models/GenerateDescription';

@Injectable({
  providedIn: 'root',
})
export class GenerateService {
  private api = inject(RestApiService);

  async generateDescription(prompt: string): Promise<GenerateDescriptionResponse> {
    const body: GenerateDescriptionRequest = {
      prompt,
      max_tokens: 128,
    };

    return await this.api.post<GenerateDescriptionResponse>('/api/generate/description', body);
  }

  async generateChatReply(prompt: string): Promise<GenerateDescriptionResponse> {
    const body: GenerateDescriptionRequest = {
      prompt,
      max_tokens: 64,
    };

    return await this.api.post<GenerateDescriptionResponse>('/api/generate/description', body);
  }
}
