import functools
import time
import logging

# Configure logging 
logging.basicConfig(
    level=logging.INFO,
    format="%(asctime)s - %(levelname)s - %(message)s"
)


# Log Function Calls
def log_call(func):

    @functools.wraps(func)
    def wrapper(*args, **kwargs):
        logging.info(f"Calling function: {func.__name__}")
        result = func(*args, **kwargs)
        logging.info(f"Finished function: {func.__name__}")
        return result

    return wrapper



# Retry Decorator

def retry(times=3, delay=1):
    """
    Retries a function if it raises an exception

    Args:
        times (int): number of attempts
        delay (int): delay between retries (seconds)
    """
    def decorator(func):
        @functools.wraps(func)
        def wrapper(*args, **kwargs):
            last_exception = None

            for attempt in range(1, times + 1):
                try:
                    logging.info(f"Attempt {attempt} for {func.__name__}")
                    return func(*args, **kwargs)

                except Exception as e:
                    last_exception = e
                    logging.warning(
                        f"{func.__name__} failed on attempt {attempt}: {e}"
                    )

                    if attempt < times:
                        time.sleep(delay)
                    else:
                        logging.error(
                            f"{func.__name__} failed after {times} attempts"
                        )
                        raise last_exception

        return wrapper

    return decorator