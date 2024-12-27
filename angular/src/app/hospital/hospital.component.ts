import { ListService, PagedAndSortedResultRequestDto, PagedResultDto } from '@abp/ng.core';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HospitalDto } from '@proxy/hospital-dtos';
import { ProvinceDto } from '@proxy/province-dtos/models';
import { CommuneService, DistrictService, HospitalService, ProvinceService } from '@proxy/service';
import { NzNotificationService } from 'ng-zorro-antd/notification';

@Component({
  selector: 'app-hospital',
  templateUrl: './hospital.component.html',
  styleUrl: './hospital.component.scss',
  providers: [ListService],
})
export class HospitalComponent implements OnInit {
  hospital = { items: [], totalCount: 0 } as PagedResultDto<HospitalDto>;

  provinces = [];
  districts = [];
  communes = [];

  form: FormGroup;

  isModalOpen = false;

  selectedHospital = {} as HospitalDto;

  currentPage = 1;
  pageSize = 10;

  constructor(
    public readonly list: ListService,
    private hospitalService: HospitalService,
    private provinceService: ProvinceService,
    private districtService: DistrictService,
    private communeService: CommuneService,
    private fb: FormBuilder,
    private notification: NzNotificationService
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

    this.hospitalService.getList(query).subscribe(response => {
      this.hospital = response;
    });
  }

  loadProvinces() {
    const provinceStreamCreator = query => this.provinceService.getList(query);

    this.list.hookToQuery(provinceStreamCreator).subscribe(response => {
      this.provinces = response.items;
    });
  }

  onProvinceChange(provinceCode: number) {
    this.districts = [];
    this.communes = [];
    this.districtService
      .getDistrictByProvinceCodeByProvinceCode(provinceCode)
      .subscribe(response => {
        this.districts = response;
      });
  }

  onDistrictChange(districtCode: number) {
    this.communes = [];
    this.communeService.getCommuneByDistrictCodeByDistrictCode(districtCode).subscribe(response => {
      this.communes = response;
    });
  }

  createHospital() {
    this.selectedHospital = {} as HospitalDto;
    this.buildForm();
    this.isModalOpen = true;
  }

  editHospital(id: string) {
    this.hospitalService.get(id).subscribe(item => {
      this.selectedHospital = item;
      this.buildForm();
      this.onProvinceChange(this.selectedHospital.provinceCode);
      this.onDistrictChange(this.selectedHospital.districtCode);
      this.isModalOpen = true;
    });
  }

  buildForm() {
    this.form = this.fb.group({
      hospitalCode: [
        this.selectedHospital.hospitalCode || null,
        [Validators.required, Validators.min(1)],
      ],
      hospitalName: [
        this.selectedHospital.hospitalName || '',
        [Validators.required, Validators.minLength(3), Validators.maxLength(128)],
      ],
      provinceCode: [this.selectedHospital.provinceCode || null, [Validators.required]],
      districtCode: [this.selectedHospital.districtCode || null, [Validators.required]],
      communeCode: [this.selectedHospital.communeCode || null, [Validators.required]],
      address: [
        this.selectedHospital.address || '',
        [Validators.required, Validators.minLength(5), Validators.maxLength(128)],
      ],
    });
  }
  save() {
    if (this.form.invalid) {
      return;
    }

    const request = this.selectedHospital.id
      ? this.hospitalService.update(this.selectedHospital.id, this.form.value)
      : this.hospitalService.create(this.form.value);

    request.subscribe(() => {
      this.isModalOpen = false;
      this.form.reset();
      // this.list.get();
      this.fetchData();
      this.notification.success(
        'Success',
        this.selectedHospital.id ? 'updated successfully' : 'created successfully'
      );
    });
  }

  delete(id: string) {
    // this.confirmation.warn('::AreYouSureToDelete', '::AreYouSure').subscribe(status => {
    //   if (status === Confirmation.Status.confirm) {

    //   }
    // });
    this.hospitalService.delete(id).subscribe(() => this.fetchData());
    this.notification.success('Success', 'deleted successfully');
  }

  closeModal() {
    this.isModalOpen = false;
  }

  onPageChange(page: number) {
    this.currentPage = page;
    this.fetchData();
  }
}
