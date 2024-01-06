export const nameOf = (f: () => string) => {
    const result = (f).toString().replace(/[ |\(\)=>]/g,'')
    if(result.indexOf('.')) {
      return result.split(".").pop() ?? ''
    }
    return result
};