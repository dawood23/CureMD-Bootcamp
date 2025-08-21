import { TestBed } from '@angular/core/testing';

import { DataIndexService } from '../service/data-index.service';

describe('DataIndexService', () => {
  let service: DataIndexService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DataIndexService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
