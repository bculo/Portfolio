import { WebAddressPipe } from './web-address.pipe';

describe('WebAddressPipe', () => {
  it('create an instance', () => {
    const pipe = new WebAddressPipe();
    expect(pipe).toBeTruthy();
  });
});
