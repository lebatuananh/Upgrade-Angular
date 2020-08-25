import { DatePipe } from '@angular/common';

export class ArticleHighlightModel {
}

export class ArticleHighlightSearchModel {
    public pageIndex: number;
    public pageSize: number;
    public articletype: number;
    public displaytype: number;
    public articleid: number;
    public ListData: any;
    public Pager: any;
}