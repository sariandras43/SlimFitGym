// worker-page.component.ts
import { Component, OnInit } from '@angular/core';
import { ZXingScannerModule } from '@zxing/ngx-scanner';
import { BrowserQRCodeReader } from '@zxing/library';
import { EntryService } from '../../../Services/entry.service';
import { Entry } from '../../../Models/entry.model';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-worker-page',
  imports: [ZXingScannerModule, DatePipe],
  templateUrl: './worker-page.component.html',
  styleUrl: './worker-page.component.scss',
})
export class WorkerPageComponent implements OnInit {
  scannedResult: string = '';
  entries: Entry[] = [];
  scanner: BrowserQRCodeReader | null = null;
  hasCameraPermission: boolean | null = null;
  lastEntry: Entry | null = null;
  errorMessage: string | null = null;

  constructor(private entryService: EntryService) {}
  async ngOnInit() {
    this.entryService.displayEntries$.subscribe((entries) => {
      if (entries.length > 0) {
        this.lastEntry = entries[0];
        this.entries = entries.sort(e => e.entryDate.getTime());
      }
    });

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
        accountName: qrData.name,
        entryDate: new Date(),
      };

      this.entryService.enter(entry).subscribe({
        next: () => {
          this.scannedResult = result;
          this.errorMessage = null;
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
