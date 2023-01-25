import {Component, ElementRef, ViewChild, Input } from '@angular/core';

import {IDateParams} from '@ag-grid-community/core';
import {IDateAngularComp} from '@ag-grid-community/angular';
import flatpickr from "flatpickr";

@Component({
    selector: 'app-custom-date',
    templateUrl: './custom-date.component.html',
    styleUrls: ['./custom-date.component.css']
  })

export class CustomDateComponent implements IDateAngularComp {
   @ViewChild("flatpickrEl", {read: ElementRef}) flatpickrEl: ElementRef;
   @ViewChild("eInput", {read: ElementRef}) eInput: ElementRef;

   private date: Date;
   private params: any;
   private picker: any;

   agInit(params: any): void {
       this.params = params;
       this.params.filterParams.onChange.valueChanges.subscribe(
            (value : any) => this.addFlatpickr(value));
   }

   ngAfterViewInit(): void {
        this.addFlatpickr(this.params.filterParams.value.value);
    }

   ngOnDestroy() {
       console.log(`Destroying DateComponent`);
   }

   onDateChanged(selectedDates: any) {
       this.date = selectedDates[0] || null;
       this.params.onDateChanged();
   }

   getDate(): Date {
       return this.date;
   }

   setDate(date: Date): void {
       this.date = date || null;
       this.picker.setDate(date);
   }

   setInputPlaceholder(placeholder: string): void {
       this.eInput.nativeElement.setAttribute('placeholder', placeholder);
   }

   setInputAriaLabel(label: string): void {
       this.eInput.nativeElement.setAttribute('aria-label', label);
   }

   private addFlatpickr(date : any){
        let minDate = new Date(date);
        let maxDate = new Date(minDate);
        maxDate.setMonth(maxDate.getMonth() + 1);
        maxDate.setDate(maxDate.getDate() - 1);


        this.picker = flatpickr(this.flatpickrEl.nativeElement, {
            onChange: this.onDateChanged.bind(this),
            wrap: true,
            minDate: minDate,
            maxDate: maxDate
        });

        this.picker.calendarContainer.classList.add('ag-custom-component-popup');  
   }
}