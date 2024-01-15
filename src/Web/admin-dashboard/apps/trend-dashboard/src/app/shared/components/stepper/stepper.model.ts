export interface Step {
    key: string;
    label: string;
  }
  
  export interface ActiveStep extends Step {
    index: number;
  }