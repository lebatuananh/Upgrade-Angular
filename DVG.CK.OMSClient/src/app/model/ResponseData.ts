export class ResponseData {
    public Content: string;
    public Data: object;
    public ErrorCode: number;
    public Message: string;
    public Success: boolean;
    public ReturnUrl: string;
    public Total: number;
    public TotalRow: number;
    public Type: number;
    public Roles: string;
    public Token: string;
    public RefreshToken: boolean;
  }

  export class Pager {
    public TotalItem: number;
    public TotalPage: number;
    public PageSize: number;
    public CurrentPage: number;
  }
