export enum ProductStates {
    New = 1 << 0, //1
    Approved = 1 << 1, //2
    NotApproved = 1 << 2, //4
    Deleted = 1 << 3, //8
    Disapprove = 1 << 4, //16
    Highlight = 1 << 5, //32
    Update = 1 << 6, //64,                    
    AutoUp = 1 << 7, //128
    Saled = 1 << 8, //256
    Notified = 1 << 9, //512
    WaitApprove = 1 << 10, //1024
    NotEnoughMoney = 1 << 11, //2048
    Duplicate = 1 << 12, //4096 
}
