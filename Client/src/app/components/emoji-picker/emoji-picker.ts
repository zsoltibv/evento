import { Component, ElementRef, Input, signal, ViewChild, WritableSignal } from '@angular/core';
import { PickerModule } from '@ctrl/ngx-emoji-mart';

@Component({
  selector: 'app-emoji-picker',
  imports: [PickerModule],
  templateUrl: './emoji-picker.html',
  styleUrl: './emoji-picker.scss',
})
export class EmojiPicker {
  isOpened = signal(false);

  @Input() emojiSignal!: WritableSignal<string>;
  @ViewChild('container') container: ElementRef<HTMLElement> | undefined;

  emojiSelected(event: any) {
    this.emojiSignal.set(this.emojiSignal() + event.emoji.native);
  }

  eventHandler = (event: Event) => {
    if (!this.container?.nativeElement.contains(event.target as Node)) {
      this.isOpened.set(false);
      window.removeEventListener('click', this.eventHandler);
    }
  };

  toggled() {
    console.log('hey');

    this.isOpened.set(!this.isOpened());
    if (this.isOpened()) {
      window.addEventListener('click', this.eventHandler);
    } else {
      window.removeEventListener('click', this.eventHandler);
    }
  }
}
