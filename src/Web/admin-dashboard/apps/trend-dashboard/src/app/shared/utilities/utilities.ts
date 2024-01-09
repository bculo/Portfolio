export const nameOf = (f: () => string) => {
    const result = (f).toString().replace(/[ |\(\)=>]/g,'')
    if(result.indexOf('.')) {
      return result.split(".").pop() ?? ''
    }
    return result
};

export function dateDiffInDays(dateOne: Date | string, dateTwo: Date) {
  const _MS_PER_DAY = 1000 * 60 * 60 * 24;
  if(dateOne instanceof Date) {
    const utcInner1 = Date.UTC(dateOne.getFullYear(), dateOne.getMonth(), dateOne.getDate());
    const utcInner2 = Date.UTC(dateTwo.getFullYear(), dateTwo.getMonth(), dateTwo.getDate());
    return Math.floor((utcInner2 - utcInner1) / _MS_PER_DAY);
  }
  const date = new Date(dateOne);
  const utc1 = Date.UTC(date.getFullYear(), date.getMonth(), date.getDate());
  const utc2 = Date.UTC(dateTwo.getFullYear(), dateTwo.getMonth(), dateTwo.getDate());
  return Math.floor((utc2 - utc1) / _MS_PER_DAY);
}