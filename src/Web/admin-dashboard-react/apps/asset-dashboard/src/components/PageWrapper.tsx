import React from 'react';

type Props = {
  children: React.ReactNode;
};

export const PageWrapper = ({ children }: Props) => {
  return <div className="h-full w-3/4 m-auto">{children}</div>;
};
