export interface ModalInstance {
    open(): void;
    close(): void;
    isOpen: boolean;
    id: string;
}