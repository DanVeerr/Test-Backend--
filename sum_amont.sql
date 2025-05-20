CREATE OR REPLACE FUNCTION 
	get_daily_amounts(_clientid BIGINT, _dateFrom DATE, _dateTo DATE)
RETURNS TABLE (dt DATE, amount_sum NUMERIC)
LANGUAGE sql
AS $$
    SELECT gs.dt,
           COALESCE(SUM(c.amount), 0::money) AS amount_sum
    FROM  generate_series(_dateFrom, _dateTo, INTERVAL '1 day') AS gs(dt)
    LEFT  JOIN clientpayments AS c
           ON   c.clientid = _clientid
          AND   c.dt::date  = gs.dt
    GROUP BY gs.dt
    ORDER BY gs.dt;
$$;

/*Пример использования*/
SELECT * FROM get_daily_amounts(1, '2021-01-02', '2023-01-07');