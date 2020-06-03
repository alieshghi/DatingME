export interface IPagination{
    curentPage: number;
    pageSize: number;
    totalCountOfItems: number;
    totalPage: number;
}
export class PaginationResult <T>
{
    pageResult: T;
    paginationResult: IPagination;
}
