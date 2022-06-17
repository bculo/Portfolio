import { KeyValuePair } from "src/app/models/backend/dictionary";
import { ControlItem } from "src/app/models/frontend/controls";

export function mapKeyValuePairCollectionToControlItems<TKey, TValue>(items: KeyValuePair[]): ControlItem[] {
    if(!items) return [];
    return items.map((item) => ({ value: item.key, displayValue: item.value }));
}