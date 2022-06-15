import { TestBed } from '@angular/core/testing';

import { FormMapperService } from './form-mapper.service';

describe('FormMapperService', () => {
  let service: FormMapperService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(FormMapperService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
