import { Component } from '@angular/core';
import { EntryService } from '../../../Services/entry.service';
import { UserService } from '../../../Services/user.service';
import { Router } from '@angular/router';
import { Entry } from '../../../Models/entry.model';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-entries',
  imports: [DatePipe],
  templateUrl: './entries.component.html',
  styleUrl: './entries.component.scss',
})
export class EntriesComponent {
  previous() {
    this.offset = this.offset - this.limit;
    this.refresh();
  }
  next() {
    this.offset = this.offset + this.limit;
    this.refresh();
  }
  scannedResult: string = '';
  hasMore = false;
  entries: Entry[] = [];
  hasCameraPermission: boolean | null = null;
  offset = 0;
  limit = 10;
  constructor(
    private entryService: EntryService,
    private userService: UserService,
    private router: Router
  ) {}
  refresh() {
    this.entryService.getEntries({
      limit: this.limit,
      offset: this.offset,
      orderField: 'date',
    });
  }
  async ngOnInit() {
    this.entryService.displayEntries$.subscribe((entries) => {
      if (entries.length > 0) {
        this.entries = entries.sort((e) => e.entryDate.getTime());
        this.hasMore = this.entries.length == this.limit;
      }
    });
    this.refresh();
  }
}
