export const sessionMemory = {
   get(key: string): any {
      const value = typeof window !== 'undefined' ? sessionStorage.getItem(key) : null;
      if (value) return value;
   },

   set(key: string, value: any) {
      sessionStorage.setItem(key, value);
   },

   remove(key: string) {
      sessionStorage.removeItem(key);
   },
};
