import { TestBed } from '@angular/core/testing';

import { SyncResolver } from './sync.resolver';

describe('SyncResolver', () => {
  let resolver: SyncResolver;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    resolver = TestBed.inject(SyncResolver);
  });

  it('should be created', () => {
    expect(resolver).toBeTruthy();
  });
});
