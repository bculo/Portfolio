import { AfterViewInit, Component, Input, OnDestroy, OnInit, ViewChild, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SearchWordStore } from '../../store/search-word-store';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { toObservable } from '@angular/core/rxjs-interop';
import { Subject, takeUntil, tap } from 'rxjs';
import { FormFieldComponent } from 'apps/trend-dashboard/src/app/shared/controls/form-field/form-field.component';
import { InputComponent } from 'apps/trend-dashboard/src/app/shared/controls/input/input.component';
import { ButtonComponent } from 'apps/trend-dashboard/src/app/shared/components/button/button.component';
import { ContextTypeEnumOptions, SearchEngineEnumOptions } from 'apps/trend-dashboard/src/app/shared/enums/enums';
import { SelectComponent } from 'apps/trend-dashboard/src/app/shared/controls/select/select.component';
import { DictionaryStore } from 'apps/trend-dashboard/src/app/store/dictionary/dictionary-store';

import { NgApexchartsModule } from "ng-apexcharts";
import {
  ApexNonAxisChartSeries,
  ApexPlotOptions,
  ApexChart,
  ApexFill,
  ChartComponent,
  ApexStroke
} from "ng-apexcharts";

export type ChartOptions = {
  series: ApexNonAxisChartSeries;
  chart: ApexChart;
  labels: string[];
  plotOptions: ApexPlotOptions;
};

@Component({
  selector: 'admin-dashboard-search-word-detail',
  standalone: true,
  imports: 
    [CommonModule, FormFieldComponent, InputComponent, SelectComponent, ButtonComponent,
     ReactiveFormsModule, NgApexchartsModule],
  templateUrl: './search-word-detail.component.html',
  styleUrl: './search-word-detail.component.scss',
})
export class SearchWordDetailComponent implements OnInit, OnDestroy {
  readonly searchWordStore = inject(SearchWordStore);
  readonly dictionaryStore = inject(DictionaryStore);
  readonly formBuilder = inject(FormBuilder);

  @ViewChild('chart') chart!: ChartComponent;
  public chartOptions!: Partial<ChartOptions>;

  updateItem = this.searchWordStore.updateItem;
  contextTypes = this.dictionaryStore.contextTypeEditItemsOptions;
  searchEngines = this.dictionaryStore.searchEngineEditItemsOptions;

  isUpdateMode = this.updateItem() != null;

  form: FormGroup = this.formBuilder.group({
    searchWord: new FormControl<string | null>(null, { updateOn: 'change', validators: [Validators.required] }),
    contextType: new FormControl<number>(ContextTypeEnumOptions.Crypto, { updateOn: 'change', validators: [Validators.required] }),
    searchEngine: new FormControl<number>(SearchEngineEnumOptions.Google, { updateOn: 'change', validators: [Validators.required] }),
  });

  ngOnInit(): void {
    this.prepareUpdateMode();

    this.chartOptions = {
      series: [70],
      chart: {
        height: '200',
        type: "radialBar",
        foreColor: '#fff'
      },
      plotOptions: {
        radialBar: {
          hollow: {
            size: "70%"
          }
        }
      },
      labels: ["Sync"]
    };
  }

  private prepareUpdateMode() {
    const item = this.updateItem();

    if(!item) {
      return;
    }

    this.form.patchValue({
      ...item,
      'contextType': item.contextTypeId,
      'searchEngine': item.searchEngineId,
    });
  }

  ngOnDestroy(): void {

  }

  onSubmit(): void {
    console.log(this.form.value)
  }
}