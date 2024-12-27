import { ListService, PagedAndSortedResultRequestDto, PagedResultDto } from '@abp/ng.core';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PantientDto } from '@proxy/pantient-dtos';
import {
  CommuneService,
  DistrictService,
  HospitalService,
  PantientService,
  ProvinceService,
} from '@proxy/service';
import { NzNotificationService } from 'ng-zorro-antd/notification';

@Component({
  selector: 'app-patient',
  templateUrl: './patient.component.html',
  styleUrl: './patient.component.scss',
  providers: [ListService],
})
export class PatientComponent implements OnInit {
  patient = { items: [], totalCount: 0 } as PagedResultDto<PantientDto>;

  provinces = [];
  districts = [];
  communes = [];

  form: FormGroup;

  isModalOpen = false;

  selectedPatient = {} as PantientDto;

  currentPage = 1;
  pageSize = 10;
  constructor(
    public readonly list: ListService,
    private hospitalService: HospitalService,
    private provinceService: ProvinceService,
    private districtService: DistrictService,
    private communeService: CommuneService,
    private patientService: PantientService,
    private fb: FormBuilder,
    private notification: NzNotificationService
  ) {}

  ngOnInit(): void {
    this.fetchData();
    this.loadProvinces();
  }

  buildForm() {
    this.form = this.fb.group({
      code: [this.selectedPatient.code || '', [Validators.required]],
      name: [
        this.selectedPatient.name || '',
        [Validators.required, Validators.minLength(3), Validators.maxLength(128)],
      ],
      provinceCode: [this.selectedPatient.provinceCode || null, [Validators.required]],
      districtCode: [this.selectedPatient.districtCode || null, [Validators.required]],
      communeCode: [this.selectedPatient.communeCode || null, [Validators.required]],
      address: [
        this.selectedPatient.address || '',
        [Validators.required, Validators.minLength(5), Validators.maxLength(128)],
      ],
    });
  }

  fetchData() {
    const query: PagedAndSortedResultRequestDto = {
      sorting: '', // Add sorting logic if needed
      skipCount: (this.currentPage - 1) * this.pageSize,
      maxResultCount: this.pageSize,
    };

    this.patientService.getList(query).subscribe(response => {
      this.patient = response;
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

  createPatient() {
    this.selectedPatient = {} as PantientDto;
    this.buildForm();
    this.isModalOpen = true;
  }

  editPatient(id: string) {
    this.patientService.get(id).subscribe(item => {
      this.selectedPatient = item;
      this.buildForm();
      this.onProvinceChange(this.selectedPatient.provinceCode);
      this.onDistrictChange(this.selectedPatient.districtCode);
      this.isModalOpen = true;
    });
  }

  save() {
    if (this.form.invalid) {
      return;
    }

    const request = this.selectedPatient.id
      ? this.patientService.update(this.selectedPatient.id, this.form.value)
      : this.patientService.create(this.form.value);

    request.subscribe(() => {
      this.isModalOpen = false;
      this.form.reset();
      // this.list.get();
      this.fetchData();
      this.notification.success(
        'Success',
        this.selectedPatient.id ? 'updated successfully' : 'created successfully'
      );
    });
  }

  delete(id: string) {
    // this.confirmation.warn('::AreYouSureToDelete', '::AreYouSure').subscribe(status => {
    //   if (status === Confirmation.Status.confirm) {

    //   }
    // });
    this.patientService.delete(id).subscribe(() => this.fetchData());
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
