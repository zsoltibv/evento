import { CommonModule } from '@angular/common';
import { Component, output, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { InputTextModule } from 'primeng/inputtext';

@Component({
  selector: 'app-search-bar',
  imports: [CommonModule, FormsModule, InputTextModule],
  templateUrl: './search-bar.html',
  styleUrl: './search-bar.scss',
})
export class SearchBar {
  query = signal('');
  search = output<string>();

  onInput(value: string): void {
    this.query.set(value);
    this.search.emit(value.trim());
  }

  clear(): void {
    this.query.set('');
    this.search.emit('');
  }
}
