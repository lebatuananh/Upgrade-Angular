export class Stateshelper {
    public static HasState(state, comparestate) {
        return (state & comparestate) == comparestate;
    }
    public static TurnOnState(sourcestate, turnonstate) {
        return sourcestate | turnonstate;
    }
    public static TurnOffState(sourcestate, turnoffstate) {
        return sourcestate & ~turnoffstate;
    }
}
