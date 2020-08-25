export class DateTimeHelper {
  public static GetListMonthSelect() {
    const listMonth: any[] = [];
    for (let i = 1; i < 13; i++) {
      switch (i) {
        case 1:
          listMonth.push({ Name: 'January', Value: i });
          break;
        case 2:
          listMonth.push({ Name: 'February', Value: i });
          break;
        case 3:
          listMonth.push({ Name: 'March', Value: i });
          break;
        case 4:
          listMonth.push({ Name: 'April', Value: i });
          break;
        case 5:
          listMonth.push({ Name: 'May', Value: i });
          break;
        case 6:
          listMonth.push({ Name: 'June', Value: i });
          break;
        case 7:
          listMonth.push({ Name: 'July', Value: i });
          break;
        case 8:
          listMonth.push({ Name: 'August', Value: i });
          break;
        case 9:
          listMonth.push({ Name: 'September', Value: i });
          break;
        case 10:
          listMonth.push({ Name: 'October', Value: i });
          break;
        case 11:
          listMonth.push({ Name: 'November', Value: i });
          break;
        case 12:
          listMonth.push({ Name: 'December', Value: i });
          break;
      }
    }

    return listMonth;
  }

  public static GetListYearSelect() {
    const listYear: any[] = [];
    for (let i = new Date().getFullYear(); i > 2016; i--) {
      listYear.push({ Name: i, Value: i });
    }
    listYear[0].selected = true;
    return listYear;
  }
}
