export interface SideModalInstance {
    open(): void;
    close(): void;
    isOpen: boolean;
    id: string;
}