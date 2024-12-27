import { ListService, PagedAndSortedResultRequestDto, PagedResultDto } from '@abp/ng.core';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { communeLevelOptions } from '@proxy/area/commune-level.enum';
import { CommuneDto } from '@proxy/commune-dtos';
import { ConfirmationService, Confirmation } from '@abp/ng.theme.shared';
import { CommuneService } from '@proxy/service/commune.service';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { DistrictService } from '@proxy/service';

@Component({
  selector: 'app-commune',
  templateUrl: './commune.component.html',
  styleUrl: './commune.component.scss',
  providers: [ListService],
})
export class CommuneComponent implements OnInit {
  commune = { items: [], totalCount: 0 } as PagedResultDto<CommuneDto>;

  form: FormGroup;

  isModalOpen = false;
  isUploadModalOpen = false;

  communeLevel = communeLevelOptions;

  selectedCommune = {} as CommuneDto;

  currentPage = 1;
  pageSize = 10;

  districts = [];

  constructor(
    public readonly list: ListService,
    private communeService: CommuneService,
    private fb: FormBuilder,
    private confirmation: ConfirmationService,
    private notification: NzNotificationService,
    private districtService: DistrictService
  ) {}

  ngOnInit() {
    // const communeStreamCreator = query => this.communeService.getList(query);

    // this.list.hookToQuery(communeStreamCreator).subscribe(response => {
    //   this.commune = response;
    // });
    this.fetchData();
    this.loadDistricts();
  }

  fetchData() {
    const query: PagedAndSortedResultRequestDto = {
      sorting: '', // Add sorting logic if needed
      skipCount: (this.currentPage - 1) * this.pageSize,
      maxResultCount: this.pageSize,
    };

    this.communeService.getList(query).subscribe(response => {
      this.commune = response;
    });
  }

  loadDistricts() {
    const districtStreamCreator = query => this.districtService.getList(query);

    this.list.hookToQuery(districtStreamCreator).subscribe(response => {
      this.districts = response.items;
    });
  }

  createCommune() {
    this.selectedCommune = {} as CommuneDto;
    this.buildForm();
    this.isModalOpen = true;
  }

  editCommune(id: string) {
    this.communeService.get(id).subscribe(item => {
      this.selectedCommune = item;
      this.buildForm();
      this.isModalOpen = true;
    });
  }

  buildForm() {
    this.form = this.fb.group({
      communeCode: [this.selectedCommune.communeCode || null, Validators.required],
      communeName: [this.selectedCommune.communeName || '', Validators.required],
      communeEnName: [this.selectedCommune.communeEnName || ''],
      communeLevel: [this.selectedCommune.communeLevel || null, Validators.required],
      districtCode: [this.selectedCommune.districtCode || null, Validators.required],
    });
  }
  save() {
    if (this.form.invalid) {
      return;
    }

    const request = this.selectedCommune.id
      ? this.communeService.update(this.selectedCommune.id, this.form.value)
      : this.communeService.create(this.form.value);

    request.subscribe(() => {
      this.isModalOpen = false;
      this.form.reset();
      // this.list.get();
      this.fetchData();

      this.notification.success(
        'Success',
        this.selectedCommune.id ? 'updated successfully' : 'created successfully'
      );
    });
  }

  delete(id: string) {
    this.confirmation.warn('::AreYouSureToDelete', '::AreYouSure').subscribe(status => {
      if (status === Confirmation.Status.confirm) {
        this.communeService.delete(id).subscribe(() => this.fetchData());
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

    this.communeService.importFileByFile(formData).subscribe({
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
