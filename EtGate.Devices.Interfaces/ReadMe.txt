Arranging this way serve two goals:
- Can have dummy implementations of the controllers. These dummy implementations will have transitive dependency only on the domain, not on the dlls' of the real SDKs
Thus no possible problem during runtime for loading (unnecessary items)
- Instead of making new dlls for each device, I packed them in one. No harm. If in some case, we realize that such is needed, can be split in individual dlls (each having one abstract class and the family of interfaces)