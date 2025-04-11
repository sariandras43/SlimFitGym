// worker-page.component.ts
import { Component, OnInit } from '@angular/core';
import { ZXingScannerModule } from '@zxing/ngx-scanner';
import { BrowserQRCodeReader } from '@zxing/library';
import { EntryService } from '../../../Services/entry.service';
import { Entry } from '../../../Models/entry.model';
import { DatePipe } from '@angular/common';
import { UserService } from '../../../Services/user.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-worker-page',
  imports: [ZXingScannerModule, DatePipe],
  templateUrl: './worker-page.component.html',
  styleUrl: './worker-page.component.scss',
})
export class WorkerPageComponent implements OnInit {
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
  scanner: BrowserQRCodeReader | null = null;
  hasCameraPermission: boolean | null = null;
  lastEntry: Entry | null = null;
  errorMessage: string | null = null;
  offset = 0;
  limit = 10;
  logout() {
    this.userService.logout();

    this.router.navigate(['/']);
  }
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
        if (!this.lastEntry) {
          this.lastEntry = entries[0];
        }


        this.entries = entries.sort((e) => e.entryDate.getTime());
        this.hasMore = this.entries.length == this.limit;
      }
    });
    this.refresh();
    try {
      await navigator.mediaDevices.getUserMedia({ video: true });
      this.hasCameraPermission = true;
      this.initializeScanner();
    } catch (error) {
      console.error('Camera error:', error);
      this.hasCameraPermission = false;
    }
  }

  initializeScanner() {
    this.scanner = new BrowserQRCodeReader();
    this.scanner
      .listVideoInputDevices()
      .then((devices) => {
        if (devices.length > 0) {
          const deviceId = devices[0].deviceId;
          this.scanner?.decodeFromVideoDevice(
            deviceId,
            'video-element',
            (result) => {
              if (result) {
                this.onCodeResult(result.getText());
              }
            }
          );
        }
      })
      .catch(console.error);
  }

  onCodeResult(result: string) {
    try {
      const qrData = JSON.parse(result);
      if (!qrData?.id || !qrData?.name) {
        throw new Error('Invalid QR code format');
      }

      const entry: Entry = {
        id: 0,
        accountId: qrData.id,
        name: qrData.name,
        entryDate: new Date(),
      };
      if (
        this.lastEntry?.id === entry.id &&
        entry.entryDate.getTime() <
          this.lastEntry.entryDate.getTime() + 10 * 6000 // returns if scanned within a minute
      ) {
        return;
      }


      this.entryService.enter(entry).subscribe({
        next: () => {
          this.scannedResult = result;
          this.errorMessage = null;
          this.lastEntry = entry;
        },
        error: (err) => {
          this.errorMessage = err.error.message || 'Failed to register entry';
        },
      });
    } catch (err) {
      this.errorMessage = 'Invalid QR code format';
      console.error('QR code parsing error:', err);
    }
  }

  ngOnDestroy() {
    this.scanner?.reset();
  }
}
