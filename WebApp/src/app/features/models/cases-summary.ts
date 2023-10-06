export interface CaseSummary {
  sampleCollectedDate: Date;
  quantityOfCases: number;
  quantityByTestType: Record<string, number>;
}
