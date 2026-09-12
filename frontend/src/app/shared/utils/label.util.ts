export function labelOf<T extends string>(map: Record<T, string>, key: T): string {
  return map[key];
}
