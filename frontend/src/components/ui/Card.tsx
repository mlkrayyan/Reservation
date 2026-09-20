import React from 'react';
import './Card.css';

export const Card: React.FC<{ children: React.ReactNode; className?: string }> = ({ children, className }) => {
  return <div className={`card ${className || ''}`}>{children}</div>;
};
