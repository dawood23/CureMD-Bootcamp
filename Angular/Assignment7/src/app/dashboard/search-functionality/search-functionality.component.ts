import { Component, EventEmitter, Output  } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { SearchBarDirective } from './search-bar.directive';

@Component({
  selector: 'app-search-functionality',
  standalone: true,
  imports: [FormsModule,SearchBarDirective],
  templateUrl: './search-functionality.component.html',
  styleUrl: './search-functionality.component.scss'
})
export class SearchFunctionalityComponent {
   searchTerm: string = '';

  @Output() search = new EventEmitter<string>();

  onSearch() {
    this.search.emit(this.searchTerm.trim());
  }
}
