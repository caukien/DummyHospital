import { ListService, PagedAndSortedResultRequestDto, PagedResultDto } from '@abp/ng.core';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { provinceLevelOptions } from '@proxy/area/province-level.enum';
import { ProvinceDto } from '@proxy/province-dtos';
import { ProvinceService } from '@proxy/service/province.service';
import { ConfirmationService, Confirmation } from '@abp/ng.theme.shared';
import { NzNotificationService } from 'ng-zorro-antd/notification';

@Component({
  selector: 'app-province',
  templateUrl: './province.component.html',
  styleUrl: './province.component.scss',
  providers: [ListService],
})
export class ProvinceComponent implements OnInit {
  isModalOpen = false;
  isUploadModalOpen = false;

  provinceLevel = provinceLevelOptions;

  form: FormGroup;

  province = { items: [], totalCount: 0 } as PagedResultDto<ProvinceDto>;

  selectedProvince = {} as ProvinceDto;

  currentPage = 1;
  pageSize = 10;

  constructor(
    public readonly list: ListService,
    private provinceService: ProvinceService,
    private fb: FormBuilder,
    private confirmation: ConfirmationService,
    private notification: NzNotificationService
  ) {}

  ngOnInit() {
    // // const provinceStreamCreator = query => this.provinceService.getList(query);
    // const provinceStreamCreator = query => {
    //   const queryParams: PagedAndSortedResultRequestDto = {
    //     sorting: '', // Add sorting logic if needed
    //     skipCount: (this.currentPage - 1) * this.pageSize,
    //     maxResultCount: this.pageSize,
    //   };

    //   return this.provinceService.getList(queryParams);
    // };

    // this.list.hookToQuery(provinceStreamCreator).subscribe(response => {
    //   this.province = response;
    // });
    this.fetchData();
  }

  fetchData() {
    const query: PagedAndSortedResultRequestDto = {
      sorting: '', // Add sorting logic if needed
      skipCount: (this.currentPage - 1) * this.pageSize,
      maxResultCount: this.pageSize,
    };

    this.provinceService.getList(query).subscribe(response => {
      this.province = response;
    });
  }

  createProvince() {
    this.selectedProvince = {} as ProvinceDto;
    this.buildForm();
    this.isModalOpen = true;
  }

  editProvince(id: string) {
    this.provinceService.get(id).subscribe(item => {
      this.selectedProvince = item;
      this.buildForm();
      this.isModalOpen = true;
    });
  }

  buildForm() {
    this.form = this.fb.group({
      code: [this.selectedProvince.code || null, Validators.required],
      provinceName: [this.selectedProvince.provinceName || '', Validators.required],
      provinceEnName: [this.selectedProvince.provinceEnName || ''],
      provinceLevel: [this.selectedProvince.provinceLevel || null, Validators.required],
    });
  }
  save() {
    if (this.form.invalid) {
      return;
    }

    const request = this.selectedProvince.id
      ? this.provinceService.update(this.selectedProvince.id, this.form.value)
      : this.provinceService.create(this.form.value);

    request.subscribe(() => {
      this.isModalOpen = false;
      this.form.reset();
      // this.list.get();
      this.fetchData();

      this.notification.success(
        'Success',
        this.selectedProvince.id ? 'Province updated successfully' : 'Province created successfully'
      );
    });
  }

  delete(id: string) {
    this.confirmation.warn('::AreYouSureToDelete', '::AreYouSure').subscribe(status => {
      if (status === Confirmation.Status.confirm) {
        this.provinceService.delete(id).subscribe(() => this.fetchData());
        this.notification.success('Success', 'Province deleted successfully');
      }
    });
  }

  closeModal() {
    this.isModalOpen = false;
  }

  onPageChange(page: number) {
    this.currentPage = page;
    this.ngOnInit();
  }

  openUploadModal() {
    this.isUploadModalOpen = true;
  }

  closeUploadModal() {
    this.isUploadModalOpen = false;
  }

  beforeUpload = (file: File): boolean => {
    const isExcel =
      file.type === 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' ||
      file.type === 'application/vnd.ms-excel';
    if (!isExcel) {
      this.notification.error('Invalid File', 'Only .xlsx or .xls files are allowed');
    }
    const isValidSize = file.size <= 5 * 1024 * 1024; // 5MB
    if (!isValidSize) {
      this.notification.error('Invalid File', 'File size must be less than 5MB');
    }
    return isExcel && isValidSize;
  };

  // Hàm xử lý upload tùy chỉnh
  customRequest = (uploadFile: any): void => {
    const formData = new FormData();
    formData.append('file', uploadFile.file);

    this.provinceService.importFileByFile(formData).subscribe({
      next: () => {
        this.notification.success('Success', 'File imported successfully');
        this.fetchData();
        this.isUploadModalOpen = false;
      },
      error: err => {
        this.notification.error('Error', 'Failed to import file');
        console.error('Import file error:', err);
      },
    });
  };
}
