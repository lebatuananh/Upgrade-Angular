export enum ArticleStates {
// tslint:disable-next-line: no-bitwise
  New = 1 << 0, // 1
// tslint:disable-next-line: no-bitwise
  Approved = 1 << 1, // 2
// tslint:disable-next-line: no-bitwise
  NotApproved = 1 << 2, // 4
// tslint:disable-next-line: no-bitwise
  Deleted = 1 << 3, // 8
// tslint:disable-next-line: no-bitwise
  Disapprove = 1 << 4, // 16
}

export enum ArticleType {
  // tslint:disable-next-line: no-bitwise
  Normal = 1,
  // tslint:disable-next-line: no-bitwise
  CarCompare = 2,
  // tslint:disable-next-line: no-bitwise
  Tips = 3,
  // tslint:disable-next-line: no-bitwise
  NewCar = 4,
  // tslint:disable-next-line: no-bitwise
  Gallery = 5,
  Stories = 6,
  Promos = 7
}
