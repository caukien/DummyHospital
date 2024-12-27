import { ListService, PagedAndSortedResultRequestDto, PagedResultDto } from '@abp/ng.core';
import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DistrictDto } from '@proxy/district-dtos';
import { DistrictService } from '@proxy/service/district.service';
import { ConfirmationService, Confirmation } from '@abp/ng.theme.shared';
import { districtLevelOptions } from '@proxy/area/district-level.enum';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { ProvinceService } from '@proxy/service';

@Component({
  selector: 'app-district',
  templateUrl: './district.component.html',
  styleUrl: './district.component.scss',
  providers: [ListService],
})
export class DistrictComponent implements OnInit {
  district = { items: [], totalCount: 0 } as PagedResultDto<DistrictDto>;

  form: FormGroup;

  isModalOpen = false;
  isUploadModalOpen = false;

  districtLevel = districtLevelOptions;

  selectedDistrict = {} as DistrictDto;

  currentPage = 1;
  pageSize = 10;

  provinces = [];

  constructor(
    public readonly list: ListService,
    private districtService: DistrictService,
    private fb: FormBuilder,
    private confirmation: ConfirmationService,
    private notification: NzNotificationService,
    private provinceService: ProvinceService
  ) {}

  ngOnInit() {
    // const districtStreamCreator = query => this.districtService.getList(query);

    // this.list.hookToQuery(districtStreamCreator).subscribe(response => {
    //   this.district = response;
    // });
    this.fetchData();
    this.loadProvinces();
  }

  fetchData() {
    const query: PagedAndSortedResultRequestDto = {
      sorting: '', // Add sorting logic if needed
      skipCount: (this.currentPage - 1) * this.pageSize,
      maxResultCount: this.pageSize,
    };

    this.districtService.getList(query).subscribe(response => {
      this.district = response;
    });
  }

  loadProvinces() {
    const provinceStreamCreator = query => this.provinceService.getList(query);

    this.list.hookToQuery(provinceStreamCreator).subscribe(response => {
      this.provinces = response.items;
    });
  }

  createDistrict() {
    this.selectedDistrict = {} as DistrictDto;
    this.buildForm();
    this.isModalOpen = true;
  }

  editDistrict(id: string) {
    this.districtService.get(id).subscribe(item => {
      this.selectedDistrict = item;
      this.buildForm();
      this.isModalOpen = true;
    });
  }

  buildForm() {
    this.form = this.fb.group({
      code: [this.selectedDistrict.code || null, Validators.required],
      districtName: [this.selectedDistrict.districtName || '', Validators.required],
      districtEnName: [this.selectedDistrict.districtEnName || ''],
      districtLevel: [this.selectedDistrict.districtLevel || null, Validators.required],
      provinceCode: [this.selectedDistrict.provinceCode || null, Validators.required],
    });
  }
  save() {
    if (this.form.invalid) {
      return;
    }

    const request = this.selectedDistrict.id
      ? this.districtService.update(this.selectedDistrict.id, this.form.value)
      : this.districtService.create(this.form.value);

    request.subscribe(() => {
      this.isModalOpen = false;
      this.form.reset();
      // this.list.get();
      this.fetchData();
      this.notification.success(
        'Success',
        this.selectedDistrict.id ? 'updated successfully' : 'created successfully'
      );
    });
  }

  delete(id: string) {
    this.confirmation.warn('::AreYouSureToDelete', '::AreYouSure').subscribe(status => {
      if (status === Confirmation.Status.confirm) {
        this.districtService.delete(id).subscribe(() => this.fetchData());
        this.notification.success('Success', 'deleted successfully');
      }
    });
  }

  closeModal() {
    this.isModalOpen = false;
  }

  onPageChange(page: number) {
    this.currentPage = page;
    this.fetchData();
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

    this.districtService.importFileByFile(formData).subscribe({
      next: () => {
        this.notification.success('Success', 'File imported successfully');
        // this.list.get();
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
