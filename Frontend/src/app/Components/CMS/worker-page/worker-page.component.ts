import { Component } from '@angular/core';
import { ZXingScannerModule } from '@zxing/ngx-scanner';


@Component({
  selector: 'app-worker-page',
  imports: [ZXingScannerModule],
  templateUrl: './worker-page.component.html',
  styleUrl: './worker-page.component.scss'
})
export class WorkerPageComponent {
  scannedResult: string = '';
  scanHistory: string[] = [];

  onCodeResult(result: string) {
    this.scannedResult = result;
    if (result && !this.scanHistory.includes(result)) {
      this.scanHistory.unshift(result);
    }
  }
}
