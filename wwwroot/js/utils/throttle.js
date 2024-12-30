export default function throttle(func, delay) {
    //It is used to detect if the function is going to be executed
    let timerFlag = null;

    return (...args) => {
        if(timerFlag === null) {
            //This prevent the function gets executed again until the time is completed
            timerFlag = setTimeout(() => {
                timerFlag = null;
            }, delay);

            //Function Execution
            return func(...args);
        }
    };
}