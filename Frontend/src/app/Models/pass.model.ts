export interface PassModel {
    id: number
    name: string
    maxEntries: number |undefined
    days: number |undefined
    price: number
    isActive?: boolean
    isHighlighted: boolean
    benefits: string[]
    validTo?: string
    remainingEntries: number |undefined
  }
  