import { HeaderSortable } from './header-sortable'
import { faSort, faSortDown, faSortUp, IconDefinition } from '@fortawesome/free-solid-svg-icons';
import { HeaderFilterable } from './header-filterable';

export class HeaderSorterDriver {

  faSortUp = faSortUp;
  faSortDown = faSortDown;
  faSort = faSort;

  private sortCallback: HeaderSortable;
  private filterCallback?: HeaderFilterable;

  private currentHeaderName?: string;
  private isAscending: boolean = true;

  private filterText: string | null = null;

  constructor(sortCallback: HeaderSortable, filterCallback?: HeaderFilterable) {
    this.sortCallback = sortCallback;
    this.filterCallback = filterCallback;
  }

  public headerClicked(headerName: string): IconDefinition {
    if (this.currentHeaderName != null && headerName === this.currentHeaderName) {
      this.isAscending = !this.isAscending;
    } else {
      this.currentHeaderName = headerName;
      this.isAscending = true;
    }
    this.sortCallback.sortBy(this.currentHeaderName, this.isAscending);
    return this.getSortIcon(this.currentHeaderName);
  }

  public getSortHeaderName(): string | undefined {
    return this.currentHeaderName;
  }

  public setFilter(filterText: string | null) {
    if (this.filterCallback == null) {
      return;
    }
    this.filterText = filterText;
    this.filterCallback.filter(this.filterText);
  }

  public getSortIcon(headerName: string): IconDefinition {
    if (this.currentHeaderName == null || headerName !== this.currentHeaderName) {
      return faSort;
    }
    return this.isAscending ? this.faSortDown : this.faSortUp;
  }
}
