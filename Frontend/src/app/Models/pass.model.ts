export interface PassModel {
  id: number;
  name: string;
  maxEntries: number | undefined;
  days: number | undefined;
  price: number;
  isActive?: boolean;
  benefits: string[];
  validTo?: string;
  remainingEntries: number | undefined;
  isHighlighted: boolean;

  //bonus for better display
  passOfUser?: boolean;
  isLoading?: boolean;
}
